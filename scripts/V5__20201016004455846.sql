CREATE TABLE ArticleTags
(
    ArticleId CHAR(36) NOT NULL FOREIGN KEY REFERENCES Articles(Id),
    TagId CHAR(36) NOT NULL FOREIGN KEY REFERENCES Tags(Id)
)
