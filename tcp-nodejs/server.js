/// Server listen of a client connect
/// send a message 'Hello, Client.' when client connected
/// server recieve data from connected client

const Net = require('net');
const port = 8080;

const clientHandler = function(socket) {
    console.log('A new connection has been established.');
    
    socket.write('Hello, client.');
    
    socket.on('data', function(chunk) {
        console.log(`Data received from client: ${chunk.toString()}`);
    });

    socket.on('end', function() {
        console.log('Closing connection with the client');
    });

    socket.on('error', function(err) {
        console.log(`Error: ${err}`);
    });
};

const server = new Net.Server();
server.on('connection', clientHandler);
server.listen(port, function() {
    console.log(`Server listening for connection requests on socket localhost:${port}`);
});
