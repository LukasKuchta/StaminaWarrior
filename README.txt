# Stamina warrior project mobile + backend

# For Db concurrency is used Optimistic checking
	- by postgres version (xmin) hidden column
	- no locking 

# do not use command/queris directly in controller api uri
 - command is for internal API only inside application
 - can contains additional logic
 - do not couple controller api with commad

# TODO 
- add integration events
- synchronization module
- mobile app
- memmory event bus
- own composition roofor each module
- use cases
- unit testing
- domain models
...