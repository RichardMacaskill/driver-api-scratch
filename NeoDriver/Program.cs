using Neo4j.Driver;
using static System.Console;

var uri = "neo4j+s://cas-testlab-neo4j.co.uk:7687";
var username = "stackoverflowuser";
var password = Environment.GetEnvironmentVariable("NEO4J_PWD");

await using var driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password));

// try to query the database!

var result = await driver.ExecuteQueryAsync(
    "MATCH p = (u:User)-[]-(po:Post)-[]-(t:Tag {tagId:'neo4j'})  " +
    "RETURN u.displayname,  po.title as Title ,po.body as Body,po.score as Score " +
    "LIMIT 100;");
 
var posts = new List<Post>();

foreach (var record in result.Records)
{

    var thisPost = new Post
    {
        UserName = record[0]?.ToString() ?? "null",
        Title = record[1]?.ToString() ?? "null",
        Body = record[2]?.ToString() ?? "null",
        Score = record[3]?.ToString() ?? "null"
    };

    posts.Add(thisPost);

    WriteLine("User: {0},Title: {1}, Body: {2}, Score: {3}", thisPost.UserName, thisPost.Title, thisPost.Body, thisPost.Score);
}

WriteLine("Done. Posts: {0}", posts.Count());

internal class Post
{
    public string? UserName { get; set; } 
    public string? Title { get; set; } 
    public string? Body { get; set; }  
    public string? Score { get; set; }  
}