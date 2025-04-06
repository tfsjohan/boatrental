# Boat Rental System

## General info

### Data repositories

Because the specification states that it will be used for different customers with different data storage
requirements, I have only created interfaces for repositories and used in-memory repositories for the
implementation. This way, it is easy to change the data storage method in the future.

### Data validation

In a real world application, I would spend more time on proper data validation. To avoid unnecessary complexity
in this task, I have only added some basic validation.

I would also add a global exception handler, because now missing required properties will throw a JSON
serialization error and not proper ModelState errors.

### Logging

No logging is used at all in the demo project.

### Testing

All functionality required for the specification have unit tests. The web api and data repositories do not
have tests because they are not part of the core assignment, but still needed to have something to show.

### Web API

The web api is very basic and only has the functionality required for the specification. It is not meant to be
used in a production environment.
