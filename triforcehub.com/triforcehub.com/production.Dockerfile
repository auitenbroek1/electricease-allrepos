FROM laravelphp/vapor:php83

###

# Add the `unixodbc-dev` library
RUN apk --update add unixodbc-dev

# Download and install MSODBCSQL packages
# https://docs.microsoft.com/en-us/sql/connect/odbc/linux-mac/installing-the-microsoft-odbc-driver-for-sql-server?view=sql-server-ver16#alpine18
RUN curl -O https://download.microsoft.com/download/b/9/f/b9f3cce4-3925-46d4-9f46-da08869c6486/msodbcsql18_18.0.1.1-1_amd64.apk
RUN curl -O https://download.microsoft.com/download/b/9/f/b9f3cce4-3925-46d4-9f46-da08869c6486/mssql-tools18_18.0.1.1-1_amd64.apk
RUN apk add --allow-untrusted msodbcsql18_18.0.1.1-1_amd64.apk
RUN apk add --allow-untrusted mssql-tools18_18.0.1.1-1_amd64.apk

# Install SQLSRV/PDO
# check https://github.com/microsoft/msphpsql/releases for latest version
RUN pecl install sqlsrv-5.10.1
RUN pecl install pdo_sqlsrv-5.10.1

# Enable SQLSRV/PDO extensions
RUN docker-php-ext-enable sqlsrv pdo_sqlsrv

###

COPY . /var/task
