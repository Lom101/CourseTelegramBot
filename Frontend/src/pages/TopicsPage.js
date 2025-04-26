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
    <div className="m-10">
      <h1 className="text-2xl mb-4">Темы внутри главы №{blockId}</h1>

      {/* Create Topic */}
      <div className="flex items-center gap-4 mb-8">
        <input
          type="text"
          value={newTopicName}
          onChange={(e) => setNewTopicName(e.target.value)}
          placeholder="Введите название темы"
          className="border p-2 mr-2"
        />
        <button onClick={handleCreateTopic} className="bg-green-500 text-white p-2 rounded">
          Добавить
        </button>
      </div>

      {/* List Topics */}
      <div className="space-y-4">
        {topics.length > 0 ? (
          topics.map((topic) => (
            <div key={topic.id} className="border p-4 rounded flex justify-between items-center bg-white">
              {editingTopicId === topic.id ? (
                <div className="flex items-center justify-between gap-4 w-full">
                <input
                  type="text"
                  value={editingTopicName}
                  onChange={(e) => setEditingTopicName(e.target.value)}
                  className="border p-2 flex-1 rounded"
                />
                <div className="flex items-center gap-2">
                  <button
                    onClick={handleUpdateTopic}
                    className="bg-blue-500 text-white px-3 py-1 rounded"
                  >
                    Сохранить
                  </button>
                  <button
                    onClick={() => setEditingTopicId(null)}
                    className="bg-gray-300 px-3 py-1 rounded"
                  >
                    Отмена
                  </button>
                </div>
              </div>
            ) : (
              <>
                <Link to={`/topic/${topic.id}`} className="text-blue-500 hover:underline">
                  {topic.title}
                </Link>
                <div className="flex items-center gap-2">
                  <button
                    onClick={() => handleEditTopic(topic.id, topic.title)}
                    className="bg-yellow-400 px-3 py-1 rounded"
                  >
                    Редактировать
                  </button>
                  <button
                    onClick={() => handleDeleteTopic(topic.id)}
                    className="bg-red-500 text-white px-3 py-1 rounded"
                  >
                    Удалить
                  </button>
                </div>
              </>
            )}
          </div>
          ))
        ) : (
          <p>Вы еще не добавили ни одну тему..</p>
        )}
      </div>
    </div>
  );
};

export default TopicsPage;
