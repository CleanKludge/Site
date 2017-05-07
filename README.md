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
docker kill $(docker ps -q); docker rm $(docker ps -a -q)

docker run -d -p 5080:80 -e "ContentRepositoryUri=https://github.com/CleanKludge/Content.git"  -e "MinimumLogLevel=Information" -e "EnableConsoleLogging=true" --name website cleankludge/website:unstable
```

### Remote
```powershell
docker run -d -p 80:80  -e "ContentRepositoryUri=<value>" -e "GitHubToken=<value>" -e "GitName=<value>" -e "GitEmail=<value>" -e "SERVER_URL=http://*:80" -e "MinimumLogLevel=Information" --name website cleankludge/website:<tag>
```