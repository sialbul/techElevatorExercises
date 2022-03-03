-- 14. The names of actors who've appeared in the movies in the "Back to the Future Collection" (28 rows)

SELECT DISTINCT person_name
FROM person
JOIN movie_actor ON person_id=actor_id
JOIN movie ON movie.movie_id= movie_actor.movie_id
JOIN collection ON movie.collection_id=collection.collection_id
WHERE collection_name ='Back to the Future Collection';