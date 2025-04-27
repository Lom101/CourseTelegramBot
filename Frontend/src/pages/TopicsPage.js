import React, { useEffect, useState } from 'react';
import { getAllTopicsByBlockId, createTopic, deleteTopic, updateTopic } from '../api/topicService';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { getBlockById } from '../api/blockService';

const TopicsPage = () => {
  const { blockId } = useParams();
  const navigate = useNavigate();

  const [topics, setTopics] = useState([]);
  const [newTopicName, setNewTopicName] = useState('');
  const [editingTopicId, setEditingTopicId] = useState(null);
  const [editingTopicName, setEditingTopicName] = useState('');


  const fetchTopics = async () => {
    try {
        try {
        const block = await getBlockById(blockId); // Сначала проверяем блок
        if (!block) {
          console.error('Block not found');
          navigate('/notfound');
          return;
        }
      }
      catch (error) {
        console.error('Error fetching block or topics', error);
        navigate('/notfound');
      }

      // Если блок найден — загружаем темы
      const topicsData = await getAllTopicsByBlockId(blockId);
      if (Array.isArray(topicsData)) {
        setTopics(topicsData);
      } 
      else {
        console.error("Unexpected topics data:", topicsData);
        setTopics([]);
      }
    }
    catch (error) {
        console.error('Error fetching block or topics', error);
      }
    };

  useEffect(() => {
    if (blockId) {
      fetchTopics();
    }
  }, [blockId]);
  

  const handleCreateTopic = async () => {
    try {
      await createTopic({title: newTopicName, blockId: blockId});
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
    <div className="min-h-screen bg-gray-50 px-10 py-8">
  <h1 className="text-3xl mb-6 font-extrabold text-gray-800 mt-12 ">Темы внутри главы №{blockId}</h1>

  {/* Create Topic */}
  <div className="flex items-center gap-4 mb-8">
    <input
      type="text"
      value={newTopicName}
      onChange={(e) => setNewTopicName(e.target.value)}
      placeholder="Введите название темы"
      className="border border-gray-300 p-3 rounded-lg w-3/4 text-gray-800 placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-gray-500"
    />
    <button
      onClick={handleCreateTopic}
      className="bg-gray-500 text-white p-3 rounded-lg hover:bg-gray-600 transition"
    >
      Добавить
    </button>
  </div>

  {/* List Topics */}
  <div className="space-y-6">
    {topics.length > 0 ? (
      topics.map((topic) => (
        <div
          key={topic.id}
          className="border border-gray-300 p-4 rounded-lg bg-white flex justify-between items-center shadow-sm hover:shadow-md transition"
        >
          {editingTopicId === topic.id ? (
            <div className="flex items-center justify-between gap-4 w-full">
              <input
                type="text"
                value={editingTopicName}
                onChange={(e) => setEditingTopicName(e.target.value)}
                className="border border-gray-300 p-3 flex-1 rounded-lg focus:outline-none focus:ring-2 focus:ring-gray-500"
              />
              <div className="flex items-center gap-2">
                <button
                  onClick={handleUpdateTopic}
                  className="bg-gray-600 text-white px-4 py-2 rounded-lg hover:bg-gray-700 transition"
                >
                  Сохранить
                </button>
                <button
                  onClick={() => setEditingTopicId(null)}
                  className="bg-gray-300 text-gray-800 px-4 py-2 rounded-lg hover:bg-gray-400 transition"
                >
                  Отмена
                </button>
              </div>
            </div>
          ) : (
            <>
              <Link
                to={`/topic/${topic.id}`}
                className="text-gray-700 hover:text-gray-900 font-semibold"
              >
                {topic.title}
              </Link>
              <div className="flex items-center gap-3">
                <button
                  onClick={() => handleEditTopic(topic.id, topic.title)}
                  className="bg-gray-400 text-white px-4 py-2 rounded-lg hover:bg-gray-500 transition"
                >
                  Редактировать
                </button>
                <button
                  onClick={() => handleDeleteTopic(topic.id)}
                  className="bg-red-400 text-white px-4 py-2 rounded-lg hover:bg-red-500 transition"
                >
                  Удалить
                </button>
              </div>
            </>
          )}
        </div>
      ))
    ) : (
      <p className="text-gray-600">Вы еще не добавили ни одну тему..</p>
    )}
  </div>
</div>

);
};

export default TopicsPage;