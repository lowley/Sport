WSL Ubuntu 22.004:/mnt/c/Users/olivier/source/repos/WhoIsPerestroikan/PerestroikanWebApp3
docker build -t whoisperestroikan3 .
docker run -d -p 5000:8080 --name wip whoisperestroikan3
localhost:5000/weatherforecast

heroku apps
heroku git:remote -a peres
heroku container:push web
heroku container:release web

pour voir ce qu'il y a dans une image
-> docker run -it --entrypoint=/bin/bash wipi

supprimer container
-> docker rm wipc1

créer container avec image
-> docker run --name wipc1 wipi

créer image (attention répertoire où on est pour le ".")
-> docker build -t wipi -f Dockerfile .
