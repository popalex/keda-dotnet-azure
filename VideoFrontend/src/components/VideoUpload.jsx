import { useState } from "react";
import axios from "axios";
import config from "../config";

const API_BASE_URL = config.API_BASE_URL;

const VideoUpload = ({ onUpload }) => {
  const [file, setFile] = useState(null);
  const [uploading, setUploading] = useState(false);

  const handleFileChange = (e) => setFile(e.target.files[0]);

  const uploadVideo = async () => {
    if (!file) return;
    setUploading(true);
    const formData = new FormData();
    formData.append("file", file);

    try {
      await axios.post(`${API_BASE_URL}/api/videos/upload`, formData);
      onUpload();
    } catch (error) {
      console.error("Upload failed", error);
    } finally {
      setUploading(false);
    }
  };

  return (
    <div className="p-6 border rounded-lg shadow flex flex-col items-center animate-fade-in">
      <input type="file" accept="video/*" onChange={handleFileChange} className="mb-2" />
      <button onClick={uploadVideo} disabled={uploading} className="px-6 py-2 bg-blue-500 rounded-lg hover:bg-blue-600 transition">
        {uploading ? "Uploading..." : "Upload Video"}
      </button>
    </div>
  );
};

export default VideoUpload;
