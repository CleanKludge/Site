# CleanKludge Website

## SetUp

```powershell
git submodule init
git submodule update
./run.ps1 setup.cake
```

## Building

```powershell
git submodule update --remote ./src/CleanKludge.Server/wwwroot/articles
./run.ps1 build.cake
```

## Publishing

```powershell
./run.ps1 publish.cake --tag=<tag>
```

## Running
### Locally

```powershell
docker run -d -p 5080:80 --name website cleankludge/website:<tag>
```

### Remote
```powershell
docker run -d -p 80:80 --name website cleankludge/website:<tag>
```