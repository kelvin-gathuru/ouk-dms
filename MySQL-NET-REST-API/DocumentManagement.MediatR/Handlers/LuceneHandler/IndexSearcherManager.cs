using System;
using System.Collections.Generic;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace DocumentManagement.MediatR.Handlers.LuceneHandler;

public class IndexSearcherManager
{
    private readonly Directory _indexDirectory;
    private IndexSearcher _searcher;

    public IndexSearcherManager(string indexPath)
    {
        _indexDirectory = FSDirectory.Open(indexPath);
    }

    public void CreateSearcher()
    {
        try
        {

            var reader = DirectoryReader.Open(_indexDirectory);
            _searcher = new IndexSearcher(reader);
        }
        catch (Exception)
        {
            var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
            var config = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer)
            {
                OpenMode = OpenMode.APPEND  // recreate index completely
            };

            using var writer = new IndexWriter(_indexDirectory, config);
            writer.Commit();
            var reader = DirectoryReader.Open(_indexDirectory);
            _searcher = new IndexSearcher(reader);
        }
    }

    public List<Guid> Search(string queryText)
    {
        var results = new List<Guid>();

        // Use StandardAnalyzer for parsing the query
        var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);

        // Create a QueryParser on the field "content"
        var parser = new QueryParser(LuceneVersion.LUCENE_48, "content", analyzer);
        Query query = parser.Parse(queryText); // Parse the query text

        // Perform the search and get the top 10 results
        var topDocs = _searcher.Search(query, 10);

        // Loop through the hits
        foreach (var scoreDoc in topDocs.ScoreDocs)
        {
            var doc = _searcher.Doc(scoreDoc.Doc);
            if (Guid.TryParse(doc.Get("id"), out var id))
            {
                results.Add(id); // Add document ID to results
            }
        }
        return results;

    }

    public void Dispose()
    {
        _indexDirectory?.Dispose();
    }
}
