docker run -e MYSQL_ROOT_PASSWORD=MySQL123! -e MYSQL_DATABASE=ef-note -p 3306:3306 --name mysql mysql:5.7

docker exec -it mysql bash

###########################

whoami
cat /etc/*release

exit
