#!/bin/sh

# Create the env-config.js file if it doesn't exist
touch /usr/share/nginx/html/env-config.js

# Clear the file content
echo "window._env_ = {" > /usr/share/nginx/html/env-config.js

# Default values
echo "  VITE_API_URL: \"/api\"," >> /usr/share/nginx/html/env-config.js

# Add all environment variables that start with VITE_
for envvar in $(env | grep -E '^VITE_' | sed -e 's/=.*//')
do
    # Get the environment variable value
    value=$(eval echo \$$envvar)
    
    # Replace the default value in env-config.js
    sed -i "s|  $envvar: .*|  $envvar: \"$value\",|" /usr/share/nginx/html/env-config.js
done

echo "}" >> /usr/share/nginx/html/env-config.js

# Ensure the file is readable
chmod 644 /usr/share/nginx/html/env-config.js 