# Cinema Reservation System – Distributed Booking Platform

## Overview

The Cinema Reservation System is a third semester software engineering project developed at VIA University College. The purpose of the project is to design and implement a distributed system that allows a cinema to manage movies, screenings, and seat reservations through a secure and user-friendly digital platform.

The system enables customers to browse available screenings and reserve seats, while cinema staff can manage movies, screenings, and reservations through administrative functionality. The solution is designed with a focus on scalability, maintainability, and integration using a multi-layered and heterogeneous architecture. 

## Features

The system provides functionality for both customers and cinema staff:

**Customer features:**

* Create an account and log in securely
* Browse available movies and screenings
* View seating layout and availability
* Reserve, edit, and cancel seat reservations
* View and manage personal reservations

**Staff and admin features:**

* Create and manage movies and screenings
* Manage cinema halls and seat layouts
* Assign roles and manage users
* Convert reservations into tickets

## Technologies Used

* Java (business logic and persistence layer)
* C# (.NET) backend with REST API
* Blazor (frontend web application)
* PostgreSQL (database)
* gRPC (communication between backend services)
* REST API (communication between frontend and backend)
* JWT authentication and password hashing for security
* UML, GRASP, and SOLID design principles

## Architecture

The system follows a distributed, multi-layer architecture:

* **Frontend (Blazor):** User interface and interaction
* **REST Backend (C#):** API gateway, authentication, and request handling
* **Service Layer (Java + gRPC):** Business logic and persistence
* **Database (PostgreSQL):** Persistent storage of system data

This architecture ensures separation of concerns, scalability, and flexibility for future expansion.

## Purpose

The project demonstrates the development of a heterogeneous distributed system using multiple programming languages, communication protocols, and architectural layers. It highlights key software engineering concepts such as system integration, service communication, secure authentication, and layered architecture.
