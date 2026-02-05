using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace DocumentManagement.MediatR.Handlers.LuceneHandler;

public class IndexWriterManager
{
    private readonly Directory _indexDirectory;
    private readonly IndexWriter _writer;

    public IndexWriterManager(string indexPath)
    {
        // Set up the directory and the writer
        if (!System.IO.Directory.Exists(indexPath))
        {
            System.IO.Directory.CreateDirectory(indexPath);
        }
        _indexDirectory = FSDirectory.Open(indexPath);
        var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
        //var analyzer = new Lucene.Net.Analysis.Core.KeywordAnalyzer();
        var indexConfig = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer);
        _writer = new IndexWriter(_indexDirectory, indexConfig);
    }

    public void AddDocument(string id, string content)
    {
        // Create a new Lucene document
        var doc = new Document
    {
        new StringField("id", id, Field.Store.YES),
        new TextField("content", content, Field.Store.YES)
    };

        // Add the document to the index
        _writer.AddDocument(doc);
        _writer.Flush(triggerMerge: false, applyAllDeletes: false);
    }

    public void Commit()
    {
        // Commit changes to the index
        _writer.Commit();
    }

    public void Dispose()
    {
        // Dispose of writer and directory
        _writer?.Dispose();
        _indexDirectory?.Dispose();
    }
}
