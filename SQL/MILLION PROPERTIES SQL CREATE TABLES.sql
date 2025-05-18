-- Table: owners
CREATE TABLE owners (
    id CHAR(24) PRIMARY KEY, -- Using CHAR(24) to represent MongoDB ObjectId
    name VARCHAR(100) NOT NULL,
    address VARCHAR(255),
    photo VARCHAR(255),
    birthday DATE
);

-- Table: properties
CREATE TABLE properties (
    id CHAR(24) PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    address VARCHAR(255),
    price DECIMAL(15, 2),
    codeInternal VARCHAR(50) UNIQUE,
    year INT,
    owner_id CHAR(24),
    CONSTRAINT fk_owner FOREIGN KEY (owner_id) REFERENCES owners(id)
        ON DELETE SET NULL ON UPDATE CASCADE
);

-- Table: propertyImages
CREATE TABLE property_images (
    id CHAR(24) PRIMARY KEY,
    property_id CHAR(24),
    file VARCHAR(255),
    enabled BOOLEAN DEFAULT TRUE,
    CONSTRAINT fk_property_image FOREIGN KEY (property_id) REFERENCES properties(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

-- Table: propertyTraces
CREATE TABLE property_traces (
    id CHAR(24) PRIMARY KEY,
    property_id CHAR(24),
    date_sale DATE,
    name VARCHAR(100),
    value DECIMAL(15, 2),
    tax DECIMAL(15, 2),
    CONSTRAINT fk_property_trace FOREIGN KEY (property_id) REFERENCES properties(id)
        ON DELETE CASCADE ON UPDATE CASCADE
);
