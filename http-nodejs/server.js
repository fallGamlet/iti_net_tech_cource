const http = require('http');

const hostname = '127.0.0.1';
const port = 8000;

const server = http.createServer(async (req, res) => {
  console.log(`${req.method} ${req.url}`);
  console.log(JSON.stringify(req.headers));
  
  const buffers = [];
  for await (const chunk of req) {
    buffers.push(chunk);
  }
  const data = Buffer.concat(buffers).toString();
  console.log(`Body: ${data}`)

  res.statusCode = 200;
  res.setHeader('Content-Type', 'text/plain');
  res.end('Hello World');
});

server.listen(port, hostname, () => {
  console.log(`Server running at http://${hostname}:${port}/`);
});
