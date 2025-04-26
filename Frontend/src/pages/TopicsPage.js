import React, { useEffect, useState } from 'react';
import { getAllTopicsByBlockId, createTopic, deleteTopic, updateTopic } from '../api/topicService';
import { Link, useParams } from 'react-router-dom';

const TopicsPage = () => {
  const { blockId } = useParams();

  const [topics, setTopics] = useState([]);
  const [newTopicName, setNewTopicName] = useState('');
  const [editingTopicId, setEditingTopicId] = useState(null);
  const [editingTopicName, setEditingTopicName] = useState('');

  const fetchTopics = async () => {
    try {
      const data = await getAllTopicsByBlockId(blockId);
      console.log('Fetched topics:', data);
      if (Array.isArray(data)) {
        setTopics(data);
      } else {
        console.error("Unexpected data format:", data);
      }
    } catch (error) {
      console.error('Failed to fetch topics', error);
    }
  };

  useEffect(() => {
    fetchTopics();
  }, []);

  const handleCreateTopic = async () => {
    try {
      await createTopic({ title: newTopicName });
      setNewTopicName('');
      fetchTopics();
    } catch (error) {
      console.error('Failed to create topic', error);
    }
  };

  const handleDeleteTopic = async (id) => {
    try {
      await deleteTopic(id);
      fetchTopics();
    } catch (error) {
      console.error('Failed to delete topic', error);
    }
  };

  const handleEditTopic = (id, name) => {
    setEditingTopicId(id);
    setEditingTopicName(name);
  };

  const handleUpdateTopic = async () => {
    try {
      await updateTopic(editingTopicId, { title: editingTopicName });
      setEditingTopicId(null);
      setEditingTopicName('');
      fetchTopics();
    } catch (error) {
      console.error('Failed to update topic', error);
    }
  };

  return (
    <div className="p-4">
      <h1 className="text-2xl mb-4">Topics</h1>

      {/* Create Topic */}
      <div className="mb-6">
        <input
          type="text"
          value={newTopicName}
          onChange={(e) => setNewTopicName(e.target.value)}
          placeholder="New Topic Name"
          className="border p-2 mr-2"
        />
        <button onClick={handleCreateTopic} className="bg-green-500 text-white p-2 rounded">
          Add Topic
        </button>
      </div>

      {/* List Topics */}
      <div className="space-y-4">
        {topics.length > 0 ? (
          topics.map((topic) => (
            <div key={topic.id} className="border p-4 rounded flex justify-between items-center">
              {editingTopicId === topic.id ? (
                <div className="flex items-center gap-2">
                  <input
                    type="text"
                    value={editingTopicName}
                    onChange={(e) => setEditingTopicName(e.target.value)}
                    className="border p-1"
                  />
                  <button onClick={handleUpdateTopic} className="bg-blue-500 text-white p-1 rounded">
                    Save
                  </button>
                  <button onClick={() => setEditingTopicId(null)} className="bg-gray-300 p-1 rounded">
                    Cancel
                  </button>
                </div>
              ) : (
                <div className="flex items-center gap-2">
                  {/* Link to topic's content page */}
                  <Link to={`/topic/${topic.id}`} className="text-blue-500 hover:underline">
                    {topic.title}
                  </Link>
                  <button onClick={() => handleEditTopic(topic.id, topic.title)} className="bg-yellow-400 p-1 rounded">
                    Edit
                  </button>
                  <button onClick={() => handleDeleteTopic(topic.id)} className="bg-red-500 text-white p-1 rounded">
                    Delete
                  </button>
                </div>
              )}
            </div>
          ))
        ) : (
          <p>No topics available</p>
        )}
      </div>
    </div>
  );
};

export default TopicsPage;
