# Stage 1 - build the Angular app
FROM node:18 AS build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build --prod

# Stage 2 - serve the Angular app using Nginx
FROM nginx:alpine
COPY --from=build /app/dist/employee-management-client /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
