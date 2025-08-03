const grpc = require('@grpc/grpc-js');
const protoLoader = require('@grpc/proto-loader');
const path = require('path');

const PROTO_PATH = path.join(__dirname, 'marketdata.proto');
const packageDefinition = protoLoader.loadSync(PROTO_PATH);
const marketdataProto = grpc.loadPackageDefinition(packageDefinition).MarketData;

function getRandomPrice(symbol) {
  // Mock price generation
  return 100 + Math.random() * 50;
}

function SubscribePrice(call) {
  const symbol = call.request.symbol;
  let count = 0;
  const interval = setInterval(() => {
    if (count++ > 100) {
      clearInterval(interval);
      call.end();
      return;
    }
    const priceUpdate = {
      symbol,
      price: getRandomPrice(symbol),
      timestamp: Date.now()
    };
    call.write(priceUpdate);
  }, 1000);

  call.on('cancelled', () => {
    clearInterval(interval);
  });
}

function main() {
  const server = new grpc.Server();
  server.addService(marketdataProto.service, { SubscribePrice });
  server.bindAsync('0.0.0.0:50051', grpc.ServerCredentials.createInsecure(), () => {
    console.log('gRPC server running at http://0.0.0.0:50051');
    server.start();
  });
}

main();
