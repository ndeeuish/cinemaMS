const fs = require('fs');
const path = require('path');

function walk(dir) {
    let results = [];
    const list = fs.readdirSync(dir);
    list.forEach(function(file) {
        file = path.join(dir, file);
        const stat = fs.statSync(file);
        if (stat && stat.isDirectory()) { 
            results = results.concat(walk(file));
        } else { 
            if(file.endsWith('.tsx')) results.push(file);
        }
    });
    return results;
}

const files = walk('d:/project/source/cinemaMS/fe/src/app');

files.forEach(file => {
    let content = fs.readFileSync(file, 'utf8');
    let changed = false;

    // Replace: error.response?.data?.details || 'fallback'
    const regex1 = /error\.response\?\.data\?\.details\s*\|\|\s*'([^']+)'/g;
    if(regex1.test(content)) {
        content = content.replace(regex1, "getErrorMessage(error, '$1')");
        changed = true;
    }

    // Replace: err.response?.data?.details || 'fallback'
    const regex2 = /err\.response\?\.data\?\.details\s*\|\|\s*'([^']+)'/g;
    if(regex2.test(content)) {
        content = content.replace(regex2, "getErrorMessage(err, '$1')");
        changed = true;
    }

    if(changed) {
        if(!content.includes('getErrorMessage')) {
            // Find last import
            const lastImportIndex = content.lastIndexOf('import ');
            if (lastImportIndex !== -1) {
                const endOfLine = content.indexOf('\n', lastImportIndex);
                content = content.slice(0, endOfLine + 1) + "import { getErrorMessage } from '@/utils/error.util';\n" + content.slice(endOfLine + 1);
            } else {
                content = "import { getErrorMessage } from '@/utils/error.util';\n" + content;
            }
        }
        fs.writeFileSync(file, content, 'utf8');
        console.log('Updated:', file);
    }
});
