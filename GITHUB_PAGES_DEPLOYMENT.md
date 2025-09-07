# GitHub Pages Deployment Guide

This guide explains how to deploy your WinstonCheckIn Blazor WebAssembly application to GitHub Pages.

## Changes Made for GitHub Pages

### 1. Removed IIS-specific files
- Deleted `web.config` files (IIS-specific configuration)
- Updated `.csproj` to exclude `web.config` from publishing

### 2. Updated for GitHub Pages routing
- Changed base href from `/` to `/WinstonCheckIn/` in `index.html`
- Created `404.html` for proper SPA routing on GitHub Pages

### 3. Created GitHub Actions workflow
- Added `.github/workflows/deploy.yml` for automated deployment
- Workflow builds and deploys on push to main/master branch

## Setup Instructions

### 1. Enable GitHub Pages
1. Go to your repository on GitHub
2. Navigate to **Settings** → **Pages**
3. Under **Source**, select **GitHub Actions**
4. Save the settings

### 2. Repository Structure
Your repository should have the following structure:
```
WinstonCheckIn/
├── .github/
│   └── workflows/
│       └── deploy.yml
├── WinstonCheckIn/
│   ├── wwwroot/
│   │   ├── index.html (with base href="/WinstonCheckIn/")
│   │   ├── 404.html
│   │   ├── .nojekyll
│   │   └── ... (other static files)
│   └── WinstonCheckIn.csproj
└── ...
```

### 3. Deployment Process
1. Push your code to the `main` or `master` branch
2. GitHub Actions will automatically:
   - Build your Blazor WebAssembly app
   - Publish it to the `wwwroot` folder
   - Deploy to GitHub Pages

### 4. Access Your Application
Your application will be available at:
`https://[your-username].github.io/WinstonCheckIn/`

## Important Notes

### Base Path Configuration
- The base href is set to `/WinstonCheckIn/` for subdirectory deployment
- If you want to deploy to the root of your GitHub Pages site, change the base href back to `/` in `index.html`

### SPA Routing
- The `404.html` file handles client-side routing for your Blazor WebAssembly app
- This ensures that direct links to routes work properly on GitHub Pages

### Jekyll Disabled
- The `.nojekyll` file tells GitHub Pages to skip Jekyll processing
- This is necessary for Blazor WebAssembly apps

### Custom Domain (Optional)
If you want to use a custom domain:
1. Add a `CNAME` file to your `wwwroot` folder with your domain name
2. Configure your domain's DNS settings to point to GitHub Pages
3. Update the base href in `index.html` to `/` instead of `/WinstonCheckIn/`

## Troubleshooting

### Build Failures
- Check the Actions tab in your GitHub repository for build logs
- Ensure all dependencies are properly referenced in your `.csproj` file

### Routing Issues
- Verify that `404.html` is present in your `wwwroot` folder
- Check that the base href matches your deployment path

### Static Assets Not Loading
- Ensure all static assets are in the `wwwroot` folder
- Check that file paths are relative to the base href

## Local Testing
To test your GitHub Pages deployment locally:

1. Build and publish your app:
   ```bash
   dotnet publish WinstonCheckIn/WinstonCheckIn.csproj --configuration Release --output ./publish
   ```

2. Serve the published files with a local server:
   ```bash
   cd publish/wwwroot
   python -m http.server 8000
   ```

3. Access your app at `http://localhost:8000/WinstonCheckIn/`

## Migration from IIS
The main differences from IIS deployment:
- No `web.config` needed (GitHub Pages uses Apache/Nginx)
- Different base path configuration
- Automated deployment via GitHub Actions instead of manual file copying
- Static file hosting instead of server-side processing
