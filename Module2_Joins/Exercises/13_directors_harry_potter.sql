-- 13. The directors of the movies in the "Harry Potter Collection" (4 rows)

SELECT DISTINCT person_name
FROM person
JOIN movie ON person_id=director_id
JOIN collection ON movie.collection_id=collection.collection_id
WHERE collection_name ='Harry Potter Collection';