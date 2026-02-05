using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Util;


namespace DocumentManagement.MediatR.Handlers.LuceneHandler
{
    public class IndexDeleteManager
    {
        private readonly Directory _indexDirectory;
        private readonly IndexWriter _writer;

        public IndexDeleteManager(string indexPath)
        {
            _indexDirectory = FSDirectory.Open(indexPath);
            var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
            var indexConfig = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer);
            _writer = new IndexWriter(_indexDirectory, indexConfig);
        }

        public bool DeleteDocumentById(string id)
        {
            Term term = new Term("id", id);
            // Delete the document(s) that match the Term
            _writer.DeleteDocuments(term);
            // Commit the changes to ensure the document is deleted
            _writer.Commit();
            return true;

        }

        public void Dispose()
        {
             _writer?.Dispose();
            _indexDirectory?.Dispose();
        }
    }
}