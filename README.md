![](images_readme/banner_API.jpg)   

# **EntityFramework-API**

## Hello and Welcome to our repository ! 👋

*******

Sommaire 
 1. [Introduction](#introduction)
 2. [Team](#team)
 3. [Installer](#installation)
 4. [Diagrams](#diagrams)



*******
<div id='introduction'/>

### **Project Introduction**

Entity Framework is an open-source object-relational mapping (ORM) tool developed by Microsoft. It enables developers to map data stored in a relational database to .NET objects. In our project, we are using Entity Framework to create a League of Legends database, and we also create a REST API based on the game to access and manipulate the data.


*******
<div id='team'/>

### **Presentation de l'équipe**

Students Second Year - BUT Informatique - IUT Clermont Auvergne - 2022-2023   
`DA COSTA CUNHA Bruno`  -  `KHEDAIR Rami` - `RANDON Noan`

*******  

<div id='installation'/>

## Functionalities

- installation

Pour installer notre projet sur Visual Studio, vous devez d'abord vous assurer que vous avez installé .NET Multi-Platform App UI (Maui) dans Visual Studio. Si vous ne l'avez pas encore installé, vous pouvez le faire en cliquant sur l'installeur de Visual Studio, puis en sélectionnant "Modifier".
![](images_readme/visual.png)  
Ensuite, sélectionnez "Développement .NET Multi-Platform App UI" dans la liste des fonctionnalités disponibles et installez-le.
![](images_readme/dev.png)  
Une fois que vous avez installé Maui, vous pouvez ouvrir le projet en relançant Visual Studio et en cliquant sur "Ouvrir un projet ou une solution" dans le menu Fichier. Sélectionnez le fichier de solution et cliquez sur "Ouvrir".
Pour exécuter le projet, vous devez sélectionner les projets "Api" et "Lol app" pour le démarrage. Pour ce faire, faites un clic droit sur la solution dans l'Explorateur de solutions, sélectionnez "Propriétés", puis sélectionnez "Plusieurs projets de démarrage". Enfin, sélectionnez "Api" et "Lol app" et cliquez sur "Appliquer".
![](images_readme/propriete.png) 
Vous pouvez maintenant lancer le projet en appuyant sur F5 ou en cliquant sur le bouton "Démarrer" dans Visual Studio.
![](images_readme/start.png) 


    
![](https://img.shields.io/badge/Entity-Framework-blue)   
![](https://img.shields.io/badge/API-Rest-Informational)


*******

<div id='conception'/>

## Diagrams

[Diagrams](On ira mettre ici le lien du diagramme)

Description des diagrammes:

Tout d'abord, pour le dossier ApiGlobale, on retrouve tous ce qui concerne l'Api, on a le projet Api, on l'on retrouve les différents controleurs soit le controleur RunePages, le controleur Runes, le controleur Skins et le controleur Champions. Nous avons réalisé une ApiRestFul de niveau 2, celle-ci respecte les contraintes de l'architecture REST (Representational State Transfer). Les contraintes de niveau 2 incluent l'utilisation d'URI pour identifier les ressources, l'utilisation des méthodes HTTP pour spécifier les actions à effectuer sur ces ressources (GET, POST, PUT, DELETE) et l'utilisation de messages auto-descriptifs. Nous avons donc tout naturellement mis en place de la pagination et du filtrage pour les méthode Get. Cependant on retrouve quelques méthodes que l'on a été contraints d'implanter pour s'adapter au client, ce qui ne reste pas les contraintes d'une ApiRestFul.
De plus nous avons utiliser swagger afin de tester notre Api. Nous voulions implémenter une Api de niveau 3, cependant la contrainte de temps nous n'a pas permis de le faire. Endin nous avons également pu déployer notre Api sur CodeFirst dans le but de la rendre accessible de n'importe ou, cela à été fait par la mise en place d'un DockerFile.
Nous avons mis en place des DTO, DTO (Data Transfer Object) est un modèle de conception qui permet de transférer des données entre des couches d'une application. 
Afin d'adapter les classes du modele à notre Api, c'est pour cela que nous avons mis en place des mapper qui permet de convertir des données d'un format à un autre, soit dans notre cas de transformer les classes du modèle en DTO avec des méthode que l'on appelé par ToDto(). Et inversement par des méthodes du genre ToSkin(), ToChampion()... 
Et pour faire cela nous avons utiliser le projet Mapper, qui permet d'effectuer ses changements, (mettre image). Afin de communiquer entre l'Api et le client nous avons utiliser le projet HTTPManager.

*******
