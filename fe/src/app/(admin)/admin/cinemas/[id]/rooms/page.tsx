'use client';

import { useEffect, useState, use } from 'react';
import { Table, Button, Modal, Form, Input, InputNumber, Space, Popconfirm, message, Tooltip, Row, Col as AntCol, Select } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, LayoutOutlined, ArrowLeftOutlined } from '@ant-design/icons';
import { adminService } from '@/services/admin.service';
import { RoomDto } from '@/types/admin.type';
import { useRouter } from 'next/navigation';

export default function AdminRoomsPage({ params }: { params: Promise<{ id: string }> }) {
  const resolvedParams = use(params);
  const cinemaId = parseInt(resolvedParams.id);
  const router = useRouter();

  const [rooms, setRooms] = useState<RoomDto[]>([]);
  const [roomTypes, setRoomTypes] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingRoom, setEditingRoom] = useState<RoomDto | null>(null);

  // Bulk generate seats state
  const [isBulkModalVisible, setIsBulkModalVisible] = useState(false);
  const [selectedRoomId, setSelectedRoomId] = useState<number | null>(null);

  const [matrixConfig, setMatrixConfig] = useState({ rows: 10, cols: 12 });
  const [seatMatrix, setSeatMatrix] = useState<number[][]>([]);

  const [form] = Form.useForm();

  const [pagination, setPagination] = useState({ current: 1, pageSize: 10, total: 0 });
  const [keyword, setKeyword] = useState<string>('');

  const fetchRooms = async (page = pagination.current, pageSize = pagination.pageSize, kw = keyword) => {
    setLoading(true);
    try {
      const data = await adminService.getRoomsByCinema(cinemaId, { pageIndex: page, pageSize, keyword: kw || undefined });
      setRooms(data.items);
      setPagination({
        current: data.pageIndex,
        pageSize: data.pageSize,
        total: data.totalItems
      });
    } catch (error) {
      message.error('Lỗi khi tải danh sách phòng chiếu');
    } finally {
      setLoading(false);
    }
  };

  const fetchTypes = async () => {
    try {
      const typesData = await adminService.getRoomTypes();
      setRoomTypes(typesData);
    } catch (error) {
      message.error('Lỗi khi tải loại phòng');
    }
  };

  useEffect(() => {
    fetchRooms(1, pagination.pageSize, keyword);
  }, [cinemaId, keyword]);

  useEffect(() => {
    fetchTypes();
  }, []);

  const handleTableChange = (newPagination: any) => {
    fetchRooms(newPagination.current, newPagination.pageSize, keyword);
  };

  const onSearch = (value: string) => {
    setKeyword(value);
  };

  // Handle matrix resizing while preserving existing seats
  useEffect(() => {
    if (!isBulkModalVisible) return;
    const newMatrix: number[][] = [];
    for (let i = 0; i < matrixConfig.rows; i++) {
      const row: number[] = [];
      for (let j = 0; j < matrixConfig.cols; j++) {
        // preserve previous state or default to 1 (Standard)
        const prevVal = seatMatrix[i]?.[j];
        row.push(prevVal !== undefined ? prevVal : 1);
      }
      newMatrix.push(row);
    }
    setSeatMatrix(newMatrix);
  }, [matrixConfig.rows, matrixConfig.cols, isBulkModalVisible]);

  const showModal = (room?: RoomDto) => {
    if (room) {
      setEditingRoom(room);
      form.setFieldsValue(room);
    } else {
      setEditingRoom(null);
      form.resetFields();
    }
    setIsModalVisible(true);
  };

  const handleFinish = async (values: any) => {
    try {
      const payload = { ...values, cinemaId };
      if (editingRoom) {
        await adminService.updateRoom(editingRoom.id, payload);
        message.success('Cập nhật phòng thành công');
      } else {
        await adminService.createRoom(payload);
        message.success('Thêm phòng chiếu thành công');
      }
      setIsModalVisible(false);
      fetchRooms();
    } catch (error: any) {
      message.error(getErrorMessage(error, 'Có lỗi xảy ra'));
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await adminService.deleteRoom(id);
      message.success('Xóa phòng chiếu thành công');
      fetchRooms();
    } catch (error: any) {
      message.error(getErrorMessage(error, 'Không thể xóa phòng chiếu này'));
    }
  };

  // Bulk Seats
  const showBulkModal = async (roomId: number) => {
    setSelectedRoomId(roomId);

    try {
      const seats = await adminService.getSeatsByRoom(roomId);
      if (seats && seats.length > 0) {
        let maxRow = 0;
        let maxCol = 0;
        seats.forEach((seat: any) => {
          const rIndex = seat.rowIndex.charCodeAt(0) - 65;
          if (rIndex > maxRow) maxRow = rIndex;
          if (seat.columnIndex > maxCol) maxCol = seat.columnIndex;
        });

        const rows = maxRow + 1;
        const cols = maxCol + 1;
        setMatrixConfig({ rows, cols });

        const newMatrix: number[][] = Array(rows).fill(0).map(() => Array(cols).fill(0));
        seats.forEach((seat: any) => {
          const rIndex = seat.rowIndex.charCodeAt(0) - 65;
          newMatrix[rIndex][seat.columnIndex] = seat.seatTypeId;
        });

        setSeatMatrix(newMatrix);
      } else {
        setMatrixConfig({ rows: 10, cols: 12 });
        setSeatMatrix([]); // Reset to trigger useEffect
      }
    } catch (error) {
      setMatrixConfig({ rows: 10, cols: 12 });
      setSeatMatrix([]); // Reset to trigger useEffect
    }

    setIsBulkModalVisible(true);
  };

  const handleCellClick = (r: number, c: number) => {
    const newMatrix = [...seatMatrix];
    const currentRow = [...newMatrix[r]];

    // Find if it's part of a couple based on our reliable logic
    let isCoupleLeft = false;
    let isCoupleRight = false;
    for (let i = 0; i < currentRow.length - 1; i++) {
      if (currentRow[i] === 3 && currentRow[i + 1] === 3) {
        if (i === c) isCoupleLeft = true;
        if (i + 1 === c) isCoupleRight = true;
        i++;
      }
    }

    // Treat clicks on the right half as clicks on the left half
    let targetC = c;
    if (isCoupleRight) {
      targetC = c - 1;
    }

    let nextVal = currentRow[targetC] + 1;
    if (nextVal > 3) nextVal = 0;

    if (nextVal === 3) {
      if (targetC < matrixConfig.cols - 1) {
        currentRow[targetC] = 3;
        currentRow[targetC + 1] = 3;
      } else {
        // Cannot make a couple at the very last column
        currentRow[targetC] = 0;
      }
    } else {
      currentRow[targetC] = nextVal;
      // If it WAS a couple before, update the right half as well
      if (isCoupleLeft || isCoupleRight) {
        currentRow[targetC + 1] = nextVal;
      }
    }

    newMatrix[r] = currentRow;
    setSeatMatrix(newMatrix);
  };

  const getSeatColor = (typeId: number) => {
    switch (typeId) {
      case 1: return 'bg-blue-500 hover:bg-blue-600 shadow-sm'; // Standard
      case 2: return 'bg-orange-500 hover:bg-orange-600 shadow-sm'; // VIP
      case 3: return 'bg-pink-500 hover:bg-pink-600 shadow-sm'; // Couple (now just normal size, since we set 2 of them)
      case 0: return 'bg-gray-100 hover:bg-gray-200 border border-dashed border-gray-300'; // Empty
      default: return 'bg-gray-100';
    }
  };

  const getSeatLabel = (typeId: number) => {
    switch (typeId) {
      case 1: return 'Thường';
      case 2: return 'VIP';
      case 3: return 'Đôi';
      case 0: return 'Trống';
      default: return '';
    }
  };

  const handleBulkGenerate = async () => {
    if (!selectedRoomId) return;
    try {
      await adminService.generateSeatsBulk(selectedRoomId, seatMatrix);
      message.success('Khởi tạo sơ đồ ghế thành công');
      setIsBulkModalVisible(false);
    } catch (error: any) {
      message.error(getErrorMessage(error, 'Lỗi khi tạo sơ đồ ghế'));
    }
  };

  const columns = [
    { title: 'Tên Phòng', dataIndex: 'name', key: 'name' },
    { title: 'Loại Phòng', dataIndex: 'roomTypeName', key: 'roomTypeName' },
    { title: 'Sức chứa (Người)', dataIndex: 'capacity', key: 'capacity' },
    {
      title: 'Hành động',
      key: 'action',
      render: (_: any, record: RoomDto) => (
        <Space size="middle">
          <Button
            type="dashed"
            icon={<LayoutOutlined />}
            onClick={() => showBulkModal(record.id)}
          >
            Sơ đồ ghế
          </Button>
          <Button type="text" icon={<EditOutlined />} onClick={() => showModal(record)} />
          <Popconfirm
            title="Bạn có chắc chắn muốn xóa phòng này?"
            onConfirm={() => handleDelete(record.id)}
            okText="Xóa"
            cancelText="Hủy"
          >
            <Button type="text" danger icon={<DeleteOutlined />} />
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div className="flex items-center mb-6 gap-4">
        <Button icon={<ArrowLeftOutlined />} onClick={() => router.push('/admin/cinemas')}>
          Quay lại
        </Button>
        <h2 className="text-2xl font-bold m-0">Phòng chiếu (Rạp ID: {cinemaId})</h2>
        <div className="ml-auto flex gap-4">
          <Input.Search
            placeholder="Tìm kiếm phòng..."
            allowClear
            onSearch={onSearch}
            style={{ width: 250 }}
          />
          <Button type="primary" icon={<PlusOutlined />} onClick={() => showModal()}>
            Thêm Phòng Mới
          </Button>
        </div>
      </div>

      <Table
        columns={columns}
        dataSource={rooms}
        rowKey="id"
        loading={loading}
        pagination={pagination}
        onChange={handleTableChange}
      />

      {/* Edit/Create Room Modal */}
      <Modal
        title={editingRoom ? 'Cập nhật Phòng' : 'Thêm Phòng Mới'}
        open={isModalVisible}
        onCancel={() => setIsModalVisible(false)}
        onOk={() => form.submit()}
      >
        <Form form={form} layout="vertical" onFinish={handleFinish}>
          <Form.Item name="name" label="Tên Phòng" rules={[{ required: true }]}>
            <Input placeholder="VD: P1, P2, IMAX" />
          </Form.Item>

          <Form.Item name="roomTypeId" label="Loại phòng" rules={[{ required: true }]}>
            <Select placeholder="Chọn loại phòng">
              {roomTypes.map(rt => (
                <Select.Option key={rt.id} value={rt.id}>{rt.name}</Select.Option>
              ))}
            </Select>
          </Form.Item>

          <Form.Item name="capacity" label="Sức chứa (Ghế)" rules={[{ required: true }]}>
            <InputNumber min={1} className="w-full" />
          </Form.Item>
        </Form>
      </Modal>

      {/* Interactive Matrix Builder Modal */}
      <Modal
        title="Công cụ Vẽ Sơ Đồ Ghế Trực Quan"
        open={isBulkModalVisible}
        onCancel={() => setIsBulkModalVisible(false)}
        onOk={handleBulkGenerate}
        okText="Lưu Sơ Đồ"
        cancelText="Hủy"
        width={900}
      >
        <div className="mb-4 text-gray-600 bg-blue-50 p-4 rounded-lg">
          <p className="font-semibold mb-2">Hướng dẫn:</p>
          <ul className="list-disc pl-5 space-y-1">
            <li>Nhập số lượng hàng và cột để tạo lưới.</li>
            <li><strong>Click vào từng ô</strong> để đổi loại ghế theo vòng tròn: Thường ➡️ VIP ➡️ Đôi ➡️ Trống (Lối đi).</li>
            <li>Hệ thống tự động bỏ qua các ô "Trống" khi đánh số ghế (VD: A1, A2, [Trống], A3).</li>
          </ul>
        </div>

        <Row gutter={16} className="mb-6">
          <AntCol span={12}>
            <div className="flex items-center gap-3">
              <span className="font-medium">Số hàng (A-Z):</span>
              <InputNumber
                min={1} max={26}
                value={matrixConfig.rows}
                onChange={(val) => setMatrixConfig(prev => ({ ...prev, rows: val || 10 }))}
              />
            </div>
          </AntCol>
          <AntCol span={12}>
            <div className="flex items-center gap-3">
              <span className="font-medium">Số ghế mỗi hàng:</span>
              <InputNumber
                min={1} max={30}
                value={matrixConfig.cols}
                onChange={(val) => setMatrixConfig(prev => ({ ...prev, cols: val || 12 }))}
              />
            </div>
          </AntCol>
        </Row>

        <div className="flex justify-center items-center gap-6 mb-6">
          <div className="flex items-center gap-2"><div className="w-6 h-6 bg-blue-500 rounded"></div> Ghế Thường</div>
          <div className="flex items-center gap-2"><div className="w-6 h-6 bg-orange-500 rounded"></div> Ghế VIP</div>
          <div className="flex items-center gap-2"><div className="w-12 h-6 bg-pink-500 rounded"></div> Ghế Đôi</div>
          <div className="flex items-center gap-2"><div className="w-6 h-6 bg-gray-100 border border-dashed border-gray-300 rounded"></div> Trống / Lối đi</div>
        </div>

        {/* The Grid Playground */}
        <div className="overflow-x-auto pb-4">
          <div className="min-w-max bg-gray-50 p-6 rounded-xl border">
            {/* Screen indicator */}
            <div className="w-3/4 mx-auto h-2 bg-gray-300 rounded-full mb-16 shadow-sm relative mt-4">
              <div className="absolute -top-6 left-1/2 -translate-x-1/2 text-gray-400 font-bold tracking-widest">MÀN HÌNH</div>
            </div>

            <div className="flex flex-col gap-2 items-center">
              {seatMatrix.map((row, rIndex) => {
                const leftCouples = new Set<number>();
                const rightCouples = new Set<number>();
                for (let i = 0; i < row.length - 1; i++) {
                  if (row[i] === 3 && row[i + 1] === 3) {
                    leftCouples.add(i);
                    rightCouples.add(i + 1);
                    i++; // skip the right part
                  }
                }

                return (
                  <div key={`row-${rIndex}`} className="flex items-center gap-2 justify-center w-full">
                    <div className="w-6 text-center font-bold text-gray-500 shrink-0">
                      {String.fromCharCode(65 + rIndex)}
                    </div>
                    <div className="flex gap-2 justify-center">
                      {row.map((seatType, cIndex) => {
                        const isCoupleLeft = leftCouples.has(cIndex);
                        const isCoupleRight = rightCouples.has(cIndex);

                        if (isCoupleRight) {
                          return null; // Skip rendering the right half
                        }

                        const widthClass = isCoupleLeft ? 'w-[72px]' : 'w-8';

                        return (
                          <Tooltip key={`seat-${rIndex}-${cIndex}`} title={`${String.fromCharCode(65 + rIndex)}${cIndex + 1} - ${getSeatLabel(seatType)}`}>
                            <div
                              className={`${widthClass} h-8 rounded cursor-pointer transition-all duration-200 flex items-center justify-center text-xs text-white relative ${getSeatColor(seatType)}`}
                              onClick={() => handleCellClick(rIndex, cIndex)}
                            >
                              {seatType === 0 ? '' : seatType === 3 ? '♥' : ''}
                            </div>
                          </Tooltip>
                        );
                      })}
                    </div>
                  </div>
                );
              })}
            </div>
          </div>
        </div>
      </Modal>
    </div>
  );
}
