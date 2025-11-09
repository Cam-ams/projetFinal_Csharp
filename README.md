# Gestion de concession
Projet C# (+ Entity Framework et Postgre)

## Consignes

### Pré-requis
- Postgres (configuré avec un utilisateur, une base de donnée crée et disponible)
- Entity Framwork

### Étape d'installation 
Adapter `appsettings.json` avec votre Postgres.

### Potentielles erreurs 
Si `appsettings.json`` n'est pas trouvé au lancement le mettre en ressource dans projetFinal.csproj

## Fonctionnalités
- Chargement des CSV `Data/voitures.csv` `Data/clients.csv` 
- Insertion des données dans la base de données
- Gestion/Historique des achats
- Calcul du prix TTC à partir du prix HT
- Gestion des clients et des voitures
- Affichage des voitures disponibles et vendues
- Réinitialisation de la base donnée

## Affichage
1) Voir liste voiture
2) Historique d'achat (croissant)
3) Ajouter un client
4) Ajouter une voiture
5) Faire un achat de voiture
6) Réinitialise la base de donnée
7) Fin


