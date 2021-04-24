docker pull mysql:5.7
docker pull mcr.microsoft.com/dotnet/sdk:5.0
docker pull mcr.microsoft.com/dotnet/aspnet:5.0

docker image ls
docker run -e MYSQL_ROOT_PASSWORD=MySQL123! -e MYSQL_DATABASE=ef-note -p 3306:3306 --name mysql mysql:5.7

###########################
USE `ef-note`;
SELECT VERSION();

###########################
CREATE TABLE user (
    id INT NOT NULL AUTO_INCREMENT,
    first_name VARCHAR(50) NOT null,
    last_name VARCHAR(50) NOT NULL,
    date_of_birth DATETIME NOT NULL,
    PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

INSERT INTO `user` VALUES (NULL, 'Jose', 'Realman', '2018-01-01');

###########################
SELECT * FROM `user`;
SHOW TABLES;
