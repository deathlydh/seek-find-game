FROM node:16-slim

WORKDIR /usr/src/app

COPY . .

RUN npm install -g http-server

EXPOSE 6060

CMD ["http-server", ".", "-p", "6060"]

