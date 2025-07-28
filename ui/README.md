# Events App UI

A Vue 3 + TypeScript + Vite application for managing events.

## Development Setup

```bash
# Install dependencies
npm install

# Start development server
npm run dev
```

## Docker Deployment

### Build the Docker Image

```bash
# Build the image
docker build -t event-management-ui .
```

### Run the Container

For PowerShell:
```powershell
docker run --detach `
    --publish 80:80 `
    --env VITE_API_URL=http://localhost:5000 `
    event-management-ui
```

For Command Prompt (CMD):
```cmd
docker run -d ^
    -p 80:80 ^
    -e VITE_API_URL=http://localhost:5000 ^
    event-management-ui
```

For Linux/Mac Terminal:
```bash
docker run -d \
    -p 80:80 \
    -e VITE_API_URL=http://localhost:5000 \
    event-management-ui
```

### Environment Variables

- `VITE_API_URL`: Base URL for the backend API (default: http://localhost:5000)

### Access the Application

Once running, access the application at:
```
http://localhost:80
```

### Troubleshooting

If you encounter issues with the environment variables not being set in Windows environments:

1. Ensure the `env.sh` script has Unix line endings (LF instead of CRLF)
2. The Dockerfile includes `dos2unix` to handle line ending conversions
3. The script permissions are set correctly with `chmod +x`

You can verify the container is running with:
```bash
docker ps
```

Expected output should show the container running with port 80 mapped:
```
CONTAINER ID   IMAGE               PORTS                               NAMES
fe0ce4d724ab   event-management-ui   0.0.0.0:80->80/tcp                event-management
```

## Learn More

- [Vue 3 Documentation](https://vuejs.org/)
- [TypeScript Guide](https://vuejs.org/guide/typescript/overview.html)
- [Vite Documentation](https://vitejs.dev/)
