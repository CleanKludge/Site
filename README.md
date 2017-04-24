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