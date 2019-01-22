PRJ = Mdb
APP = mdb
VER = `cat $(PRJ)/$(PRJ).csproj | grep -Po '(?<=<Version>)([0-9.]+?)(?=<\/Version>)'`
TAG = $(VER)

.PHONY: info
info:
	make -v
	sudo docker version --format 'Client: {{ .Client.Version }} Server: {{ .Server.Version }}'
	echo "project:"$(PRJ) "version:"$(VER) "docker-tag: "$(TAG)

.PHONY: build
build:
	rm -rf $(PRJ)/app
	dotnet publish -o app -c Release

.PHONY: build-image
build-image:
	sudo docker build -t $(APP):$(TAG) .

.PHONY: integration-tests
integration-test:
	sudo docker-compose down --rmi local
	sudo docker-compose build --no-cache
	sudo docker-compose up --abort-on-container-exit tests
	sudo docker-compose down --rmi local

.PHONY: unit-tests
unit-tests:
	ls -d *.Tests | xargs -n 1 dotnet test --filter TestCategory!=integration