-- 12. The titles of the movies in the "Star Wars Collection" that weren't directed by George Lucas (5 rows)

SELECT title
FROM movie
JOIN person ON person_id=director_id
JOIN collection ON movie.collection_id=collection.collection_id
WHERE collection_name ='Star Wars Collection' AND person_name != 'George Lucas';    