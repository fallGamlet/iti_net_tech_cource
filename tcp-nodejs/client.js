const Net = require('net');

class Client {
    constructor(id) {
        this.id = id;
        this.client = new Net.Socket();
        this.client.on('data', (chunk) => {
            console.log(`Client#${this.id}: ${chunk.toString()}\n`);
        });
        this.client.on('end', function() {
            console.log('Requested an end to the TCP connection');
        });
    }

    connect() {
        const port = 8888;
        const host = 'localhost';
        this.client.connect({ port: port, host: host }, () => {
            console.log(`Client#${this.id} connected`);
            this.client.write(`Hello, server. I am client #${this.id}`);
        });
    }

    disconnect() {
        this.client.end();
    }
}

for (let i = 0; i < 10; i++) {
    const client = new Client(i);
    client.connect();
}
