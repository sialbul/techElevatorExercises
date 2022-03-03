-- 9. The titles of movies directed by James Cameron (6 rows)

SELECT title
FROM movie
JOIN person ON person_id=director_id
WHERE person_name = 'James Cameron';