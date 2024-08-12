CREATE TABLE awing_test.problem (
  id int NOT NULL AUTO_INCREMENT,
  title TEXT NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (id)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4;

CREATE TABLE awing_test.problem_data (
  id int NOT NULL AUTO_INCREMENT,
  problem_id int NOT NULL UNIQUE,
  row int NOT NULL,
  col int NOT NULL,
  chestTypes int DEFAULT 0,
  matrix BLOB NOT NULL,
  PRIMARY KEY (id),
  CHECK (row <= 500),
  CHECK (col <= 500),
  CHECK (chestTypes <= 250000),
  FOREIGN KEY (problem_id) REFERENCES problem(id) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4;

CREATE TABLE awing_test.problem_result (
  id INT NOT NULL AUTO_INCREMENT,
  problem_id INT NOT NULL UNIQUE,
  is_resolved BOOLEAN NOT NULL,
  result DECIMAL(10, 6),
  PRIMARY KEY (id),
  FOREIGN KEY (problem_id) REFERENCES problem(id) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4;
