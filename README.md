# CleanKludge Website

## SetUp

```powershell
git submodule init
git submodule update
./run.ps1 setup
```

## Building

```powershell
git submodule update --remote ./src/CleanKludge.Server/wwwroot/articles
./run.ps1 build
```

## Publishing

```powershell
./run.ps1 publish --tag=<tag>
```

## Running
### Locally

```powershell
docker run -d -p 5080:80 --name website cleankludge/website:unstable
```

### Remote
```powershell
docker run -d -p 80:80 --name website cleankludge/website:<tag>
```