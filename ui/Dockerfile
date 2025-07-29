# build stage
FROM node:lts-alpine AS builder
WORKDIR /app

# Add build arguments - use a default value that will be replaced at runtime
ARG VITE_API_URL="/api"
ENV VITE_API_URL=$VITE_API_URL

COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build:prod

# production stage
FROM nginx:stable-alpine AS production-stage

# Install dos2unix
RUN apk add --no-cache dos2unix

# Copy custom nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy built files from builder stage
COPY --from=builder /app/dist /usr/share/nginx/html

# Add script to handle environment variables at runtime
COPY env.sh /docker-entrypoint.d/40-env.sh
RUN chmod +x /docker-entrypoint.d/40-env.sh && \
    dos2unix /docker-entrypoint.d/40-env.sh

# Expose port
EXPOSE 80
