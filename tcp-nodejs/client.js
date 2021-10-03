const Net = require('net');

/**
 * Example client class for demonstrate how to connect, send and recieve data as TCP client 
 * connected to the server
 * recieving server messages
 * sending several messages to the server
 */
class Client {
    constructor(id) {
        this.id = id;
        this.socket = new Net.Socket();
        this.socket.on('data', (chunk) => {
            console.log(`Client#${this.id}: ${chunk}\n`);
        });
        this.socket.on('end', function() {
            console.log('Requested an end to the TCP connection');
        });
    }

    connect(host, port) {
        this.socket.connect(
            { port: port, host: host }, 
            () => {
                console.log(`Client#${this.id} connected`);
                this.socket.write(`Hello, server. I am client #${this.id}`);
                this.socket.write(`client #${this.id} message 1`);
                this.socket.write(`client #${this.id} message 2`);
                this.socket.write(`client #${this.id} message 3`);
                this.socket.end()
            }
        );
    }
}

const port = 8080;
const host = 'localhost';
for (let i = 0; i < 3; i++) {
    new Client(i).connect(host, port);
}
