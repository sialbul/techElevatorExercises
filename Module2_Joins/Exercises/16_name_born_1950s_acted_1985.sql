-- 16. The names and birthdays of actors born in the 1950s who acted in movies that were released in 1985 (20 rows)

SELECT DISTINCT person_name, birthday
FROM person
JOIN movie_actor ON person_id=actor_id
JOIN movie ON movie.movie_id= movie_actor.movie_id
WHERE YEAR(birthday)>=1950 AND YEAR(birthday)<1960 AND YEAR(release_date)=1985;