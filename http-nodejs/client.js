const http = require('http');


const options = {
  port: 8000,
  host: '127.0.0.1',
  method: 'GET',
  path: '/some/path/segments/'
};
const request = http.request(options, (socket) => {
  console.log('got connected');

  socket.on('data', (chunk) => {
    console.log(chunk.toString());
  });
  socket.on('end', () => {
    console.log('connection closed');
  });
})
request.end();