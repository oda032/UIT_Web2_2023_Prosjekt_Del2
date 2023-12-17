# Prosjekt Del-2:
## Dato: 2023-12-09 - Oppstart
- Et nytt prosjekt ble etablert med Tag v1.0.0.

## Dato: 2023-12-09 - Fjerning av kommentarer:
1. Branch "feature/Comment_delete".  
Det implementeres hovedfunksjonalitet knyttet til fjerning av kommentarer.
Bare bruker som eier denne kommentaren kan fjerne den. Delete knappen er bare tilgjengelig til denne brukeren.
2. Tag v1.0.1.

## Dato: 2023-12-09 - Pop-up dialog boks ved fjerning av kommentarer:
1. Branch "feature/Comment_delete_confirmation".  
Her tas det i bruk en MudBlazor Dialog funksjonalitet for å vise en simple pop-up dialog boks ved fjerning av kommentar. 
2. Tag v1.0.2.

## Dato: 2023-12-10 - Varsling og oppdatering av alle brukere ved fjerning av kommentar:
1. Branch "feature/Comment_delete_signalR".  
Eksisterende signalR funksjonalitet for kommentarer er oppdatert. Nå kan alle brukere få varsel og oppdatert page ved fjerning av kommentar.  
2. Tag v1.0.3.

## Dato: 2023-12-14 - Forbedring av test coverage:
1. Branch "feature/Test_coverage_improvement.  
Antall tester etter forbedring er over 100 stk.
Kort beskrivelse av hva som er gjort:  
1.1 DataAccess/Repository tester. Test coverage på 95%.  
1.2 DataAccess/Data. Test coverage på 96%.  
1.3 Api/Controllers. Test coverage på 92%.  
1.4 Api/Mapping. Test coverage på 100%.  
1.5 Common/Entity. Test coverage på 82%.  
1.6 Common/Dto. Test coverage på 97%.  
1.7 Server/Service. Test coverage på 68%.

2. Tag v1.0.4.

## Dato: 2023-12-14 - Etablerer en release branch som skal holde siste oppdateringer.
1. Denne branchen skal være utgangspunktet for nye feature branches.  
Når feature er fullført, skal denne commites og et nytt pull request kjøres - her skal feature branch blir merged til denne release branch.  

## Dato: 2023-12-15 - Det vises antall innlegg og kommentarer i hvert abonnert blogg
1. Branch "feature/Extended_functionality_of_subscribed_blogs".  
Det er gjort oppdateringer i blog repository og blog controller. Blog inneholder nå alle inngående innlegg med alle inngående kommentarer.  
Betydelige endringer i design/struktur av blogItem komponent. 
Nytt header komponent for abonnert blogger.  
En test blir reparert pga endringer i api kontroller metoden.
2.  Tag v1.0.5.

## Dato: 2023-12-16 - Blogger kan "unsubscribes" når bruker ikke vil ha bloggen som subscribed.
1. Branch "feature/Unsubscribe_blogs".  
Det er lagt til en knapp for subscribed blogs som trigger "unsubscribe" prosessen.  
En UserUnsubscribedBlog entity blir fjernet i DB. Dette fører til at bloggen flytter seg til en liste med unsubscribed blogger.
Det er også lagt til et par tester som dekker nytt funksjonalitet.
2. Tag v1.0.6.

## Dato: 2023-12-17 - Helt ny funksjonalitet for å jobbe med bilder
1. Branch "feature/fileshare".  
Helt ny funksjonalitet. Brukere skal ha mulighet til å se over alle tilgjengelige bilder i fileshare.  
Hver innlogget bruker kan nå legge til et nytt bilde i fileshare.
Man benytter en Url til et bilde på nettet for å laste denne ned, og legge den til DB som en byte[].
Alle bildene hentes fra DB ved oppstart av fileshare.
2. Tag v1.1.0.



