# EmployeeManager
## Project overview
A .NET  apllication for management of salaries, payment, etc. of a company, that can have multiple offices in multiple countries. Application allows to manage salaries of the employees in different currencies. Main calculations are made in USD.
## Components of the project
Project includes 3 main services: 
- Employee Service - manages the employees, works with employee database. It is a Web API project that allows other services to gain information about offices, employees they need.
- ExchangeRate Service - service that stores information about the exchange rates of the currencies, that currently used in the system. It can gain the information from the API's of the banks to gain info, that he doesn't have at the moment. Now it takes the information from API of National Bank Of Republic of Belarus, but it has interfaces, that can be easily implemented to gain information from the different API.
- Report Service - service that can give information and statistics according to the report's rules. It also has interfaces that allow to create custom interfaces. In Basic version it has ExchangeRateOnDateRange, PaymentOnDateRange, SalarySummaryOnDate reports. It gains all information neeeded from the EmployeeService and ExchangeRate Service

Project also includes graphical interfaces for some of the services:
- Report Service web interface
- ExchangeRate Service desktop application for visualzation

There are several tests, to check the work of different services;

## Technologies used in the project 
- ASP.NET Core - used to create services and Web Interfaces
- ASP.NET Core Web API - for services projects
- WPF - used to create GUI of the Exchange Rate Service
- RazorPages - used to  visualize Report Service WebInterface
- xUnit - used to test the services

