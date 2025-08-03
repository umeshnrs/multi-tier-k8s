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

### Docker Build and Deploy

1. Build the Docker image:
   ```bash
   # From the ui directory
   docker build -t umesh3149044/ui:latest .

   # Optionally tag with specific version
   docker tag umesh3149044/ui:latest umesh3149044/ui:v1.0.0
   ```

2. Push to Docker Hub:
   ```bash
   # Push latest version
   docker push umesh3149044/ui:latest

   # Push specific version (if tagged)
   docker push umesh3149044/ui:v1.0.0
   ```

## Learn More

- [Vue 3 Documentation](https://vuejs.org/)
- [TypeScript Guide](https://vuejs.org/guide/typescript/overview.html)
- [Vite Documentation](https://vitejs.dev/)
