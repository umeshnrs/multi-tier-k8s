#!/bin/sh

# Create the env-config.js file if it doesn't exist
touch /usr/share/nginx/html/env-config.js

# Clear the file content
echo "window._env_ = {" > /usr/share/nginx/html/env-config.js

# Add all environment variables that start with VITE_
for envvar in $(env | grep -E '^VITE_' | sed -e 's/=.*//')
do
    # Get the environment variable value
    value=$(eval echo \$$envvar)
    
    # Add it to env-config.js
    echo "  $envvar: \"$value\"," >> /usr/share/nginx/html/env-config.js
done

echo "}" >> /usr/share/nginx/html/env-config.js 