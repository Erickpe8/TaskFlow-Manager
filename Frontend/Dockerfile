
FROM node:20 AS build
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build

FROM node:20 AS runtime
WORKDIR /app

COPY --from=build /app/dist/TaskFlow-Manager ./dist/TaskFlow-Manager

EXPOSE 4000

CMD ["node", "dist/TaskFlow-Manager/server/server.mjs"]
