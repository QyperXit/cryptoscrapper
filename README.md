# Crypto Data Scraper

A simple tool for scraping cryptocurrency data from CoinG3cko's DeFi Infrastructure categore pages.

## Overview

This project is designed to automate the collection of cryptocurrency data, focusing on coins categorized under DeFi Infrastructure on CoinGecko. It fetches coin names, symbols, and current prices, storing them in a MySQL database for further analysis or use in applications.

### Features

- **Web Scraping**: Uses HTML Agility Pack to parse and extract data from CoinGecko's website.
- **Database Integration**: Persists scraped data into a MySQL database using Entity Framework Core.
- **API Endpoint**: Provides a simple REST API to retrieve the scraped data.

## Setup

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version compatible with your project)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)
- [HtmlAgilityPack](https://html-agility-pack.net/) installed via NuGet
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) for database operations

### Installation

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/yourusername/your-repo-name.git
   cd your-repo-name

2. **install dependencies**

    ```bash
    dotnet restore
    ```
