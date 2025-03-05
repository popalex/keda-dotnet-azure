import { useState } from 'react'
import VideoList from './components/VideoList'
import VideoUpload from './components/VideoUpload'
import './App.css'

function App() {
  const [refresh, setRefresh] = useState(false);

  return (
    <div className="p-6 max-w-3xl mx-auto">
      <h1 className="text-2xl font-bold mb-4">🎬 Video Platform</h1>
      <VideoUpload onUpload={() => setRefresh(!refresh)} />
      <VideoList key={refresh} />
    </div>
  )
}

export default App
