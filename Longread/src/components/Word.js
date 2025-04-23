import mammoth from 'mammoth';

const Word = {
  convertToHTML: (arrayBuffer) => {
    return new Promise((resolve, reject) => {
      mammoth.convertToHtml({ arrayBuffer: arrayBuffer })
        .then((result) => {
          resolve(result.value);
        })
        .catch((error) => {
          reject(error);
        });
    });
  }
};

export default Word;
