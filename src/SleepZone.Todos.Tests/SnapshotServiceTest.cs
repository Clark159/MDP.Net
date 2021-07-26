using SleepZone.Todos.Mocks;
using System;
using System.Linq;
using Xunit;

namespace SleepZone.Todos.Tests
{
    public class SnapshotServiceTest
    {
        [Fact]
        public void Snapshot_X��Todo��ƥ]�tY�ӧ���_�ַӼƶqXY()
        {
            // Arrange
            var todoRepository = new MockTodoRepository();
            todoRepository.Add(new Todo() { TodoId = "TodoId-001", Name = "Name-001", IsComplete = true });
            todoRepository.Add(new Todo() { TodoId = "TodoId-002", Name = "Name-002", IsComplete = true });
            todoRepository.Add(new Todo() { TodoId = "TodoId-003", Name = "Name-003", IsComplete = false });

            var snapshotRepository = new MockSnapshotRepository();

            var snapshotService = new SnapshotService(todoRepository, snapshotRepository);

            // Act
            snapshotService.Snapshot();
            var snapshot = snapshotRepository.FindAll().FirstOrDefault();

            // Assert
            Assert.NotNull(snapshot);
            Assert.Equal(3, snapshot.TotalCount);
            Assert.Equal(2, snapshot.CompleteCount);
        }

        [Fact]
        public void Snapshot_X��Todo��ƥ]�t0�ӧ���_�ַӼƶqX0()
        {
            // Arrange
            var todoRepository = new MockTodoRepository();
            todoRepository.Add(new Todo() { TodoId = "TodoId-001", Name = "Name-001", IsComplete = false });
            todoRepository.Add(new Todo() { TodoId = "TodoId-002", Name = "Name-002", IsComplete = false });
            todoRepository.Add(new Todo() { TodoId = "TodoId-003", Name = "Name-003", IsComplete = false });

            var snapshotRepository = new MockSnapshotRepository();

            var snapshotService = new SnapshotService(todoRepository, snapshotRepository);

            // Act
            snapshotService.Snapshot();
            var snapshot = snapshotRepository.FindAll().FirstOrDefault();

            // Assert
            Assert.NotNull(snapshot);
            Assert.Equal(3, snapshot.TotalCount);
            Assert.Equal(0, snapshot.CompleteCount);
        }

        [Fact]
        public void Snapshot_0��Todo��ƥ]�t0�ӧ���_�ַӼƶq00()
        {
            // Arrange
            var todoRepository = new MockTodoRepository();

            var snapshotRepository = new MockSnapshotRepository();

            var snapshotService = new SnapshotService(todoRepository, snapshotRepository);

            // Act
            snapshotService.Snapshot();
            var snapshot = snapshotRepository.FindAll().FirstOrDefault();

            // Assert
            Assert.NotNull(snapshot);
            Assert.Equal(0, snapshot.TotalCount);
            Assert.Equal(0, snapshot.CompleteCount);
        }
    }
}
