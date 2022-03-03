-- 17. The titles and taglines of movies that are in the "Family" genre and Samuel L. Jackson has acted in (4 rows)


 SELECT title, tagline
 FROM movie
 JOIN movie_genre ON  movie.movie_id= movie_genre.movie_id
 JOIN genre ON movie_genre.genre_id = genre.genre_id
JOIN movie_actor ON movie.movie_id=movie_actor.movie_id
JOIN person ON person.person_id=actor_id
WHERE person_name = 'Samuel L. Jackson' AND genre_name='Family';
