import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LongreadPage from './pages/LongreadPage'; 

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/longread/:topicId" element={<LongreadPage />} />
      </Routes>
    </Router>
  );
}

export default App;